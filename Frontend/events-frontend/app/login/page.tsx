"use client";
import { useState, useEffect } from "react";
import { Input, Button, message } from "antd";
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
    setClicked(true);

    try {
      const loginResponse = await apiFetch('/login?useCookies=true', {
        method: 'POST',
        body: JSON.stringify({
          email, password, twoFactorCode: "string",
          twoFactorRecoveryCode: "string"
        }),
      });

      if (!loginResponse.ok) {
        throw new Error('Failed to log in');
      }

      const userDataResponse = await apiFetch('/api/UserData', {
        method: 'GET',
        headers: {
          'accept': 'application/json'
        },
      });

      if (userDataResponse.ok) {
        const userData: UserData = await userDataResponse.json();
        localStorage.setItem('userData', JSON.stringify(userData));
        message.success('Login successful');
        router.push("/events");
      } else {
        message.error('Failed to fetch user data');
      }
    } catch (err) {
      console.error("Error during requests:", err);
      message.error(err instanceof Error ? err.message : 'An unexpected error occurred');
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
          autoComplete="off"
          required
          placeholder="Email..."
        />
        <Input
          type="password"
          className="input"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          autoComplete="off"
          required
          placeholder="Password..."
        />
        <Button
          type="primary"
          htmlType="submit"
          className="button"
          loading={loading}
        >
          Sign In
        </Button>
        <Link href="#">Forgot password?</Link>
        <p>Not a user? <Link href="/register">Sign Up</Link></p>
      </form>
    </div>
  );
}
