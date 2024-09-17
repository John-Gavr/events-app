"use client";
import { useState } from "react";
import { Input } from "antd";
import Link from "next/link";
import { apiFetch } from "../Services/apiClient";

export default function LoginPage() {

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const data = await apiFetch('/login?useCookies=true', {
        method: 'POST',
        body: JSON.stringify({
          email, password, twoFactorCode: "string",
          twoFactorRecoveryCode: "string"
        }),
      });
      window.location.href = "/register";
    } catch (err) {
      setError('error was occured');
    } finally {
      setLoading(false);
    }
  };

  return <>
    <div className="form__wrapper">
      <h1>Sign In</h1>
      <form className="form" onSubmit={handleSubmit}>
        <Input type="email" className="input"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          autoComplete="false" required placeholder="Email..."></Input>
        <Input type="password" className="input"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          autoComplete="false" required placeholder="Password..."></Input>
        <button className="button">Sign in</button>
        <Link href="#">forgot password?</Link>
        <p>Not a user? <Link href="/register">Sign Up</Link></p>
      </form>
    </div>
  </>;
}
