"use client";

import { useState, useEffect } from "react";
import { Input, message } from "antd";
import Link from "next/link";
import { apiFetch } from "../Services/apiClient";
import { useRouter } from 'next/navigation';

interface UserData {
  id: string;
  userName: string;
  email: string;
  roles: string[];
}

export default function RegisterPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [repeatPassword, setRepeatPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
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

    if (password !== repeatPassword) {
      setError("Passwords do not match");
      setLoading(false);
      return;
    }

    setError(null);

    try {
      const response = await apiFetch('/register', {
        method: 'POST',
        body: JSON.stringify({ email, password }),
        headers: {
          'Content-Type': 'application/json'
        },
      });

      if (response.ok) {
        message.success('Registration successful! Redirecting to login...');
        router.push('/login');
      } else {
        const errorData = await response.json();
        setError(errorData.message || 'An error occurred during registration');
      }
    } catch (err) {
      console.error("Error during registration:", err);
      setError('An error occurred during registration');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="form__wrapper">
      <h1>Sign Up</h1>
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
        <Input
          type="password"
          className="input"
          value={repeatPassword}
          onChange={(e) => setRepeatPassword(e.target.value)}
          autoComplete="off"
          required
          placeholder="Re-enter password..."
        />
        <button className="button" type="submit" disabled={loading}>
          {loading ? 'Signing up...' : 'Sign up'}
        </button>
        {error && <p className="error">{error}</p>}
        <p>Already a user? <Link href="/login">Sign In</Link></p>
      </form>
    </div>
  );
}
