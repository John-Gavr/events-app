"use client";

import { useState } from "react";
import { Input } from "antd";
import Link from "next/link";
import { apiFetch } from "../Services/apiClient";

export default function RegisterPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [repeatPassword, setRepeatPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

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
      await apiFetch('/register', {
        method: 'POST',
        body: JSON.stringify({ email, password }),
      });
    } catch (err) {
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
        <Input
          type="password"
          className="input"
          value={repeatPassword}
          onChange={(e) => setRepeatPassword(e.target.value)}
          autoComplete="false"
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
