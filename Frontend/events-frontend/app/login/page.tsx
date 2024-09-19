"use client";
import { useState, useEffect } from "react";
import { Input } from "antd";
import Link from "next/link";
import { apiFetch } from "../Services/apiClient";
import './login.css';
import { useRouter } from 'next/navigation';

interface UserData {
  id: string;
  userName: string;
  email: string;
  roles: string[];
}

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [clicked, setClicked] = useState(false);

  const router = useRouter();

  useEffect(() => {
    const checkUserData = async () => {
      try {
        const userDataResponse = await apiFetch('/api/UserData', {
          method: 'GET',
          headers: {
            'accept': 'application/json'
          },
        });

        if (userDataResponse.ok) {
          const userData: UserData = await userDataResponse.json();

          localStorage.setItem('userData', JSON.stringify(userData));

          router.push("/events");
        }
      } catch (err) {
        console.error("Error fetching user data:", err);
      }
    };

    checkUserData();
  }, [router]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setClicked((clicked) => !clicked);

    try {
      if (clicked) {
        const loginResponse = await apiFetch('/login?useCookies=true', {
          method: 'POST',
          body: JSON.stringify({
            email, password, twoFactorCode: "string",
            twoFactorRecoveryCode: "string"
          }),
        });

        const userDataResponse = await apiFetch('/api/UserData', {
          method: 'GET',
          headers: {
            'accept': 'application/json'
          },
        });

        if (userDataResponse.ok) {
          const userData: UserData = await userDataResponse.json();

          localStorage.setItem('userData', JSON.stringify(userData));

          router.push("/events");
        } else {
          setError('Failed to fetch user data');
        }
      }
    } catch (err) {
      console.error("Error during requests:", err);
      if (err instanceof Error) {
        setError(err.message || 'Произошла ошибка');
      } else {
        setError('Произошла ошибка');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="form__wrapper">
      <h1>Sign In</h1>
      <form className="form" onSubmit={handleSubmit}>
        <Input
          type="email"
          className="input"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          autoComplete="false"
          required
          placeholder="Email..."
        />
        <Input
          type="password"
          className="input"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          autoComplete="false"
          required
          placeholder="Password..."
        />
        <button className="button" disabled={loading}>
          Sign in
        </button>
        <Link href="#">forgot password?</Link>
        <p>Not a user? <Link href="/register">Sign Up</Link></p>
      </form>
      {error && <p className="error">{error}</p>}
    </div>
  );
}
