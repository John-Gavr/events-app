"use client";
import { Content, Footer, Header } from "antd/es/layout/layout";
import "./globals.css";
import { Layout, Menu, message } from "antd";
import Link from "next/link";
import { useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';

const RootLayout = ({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) => {
  const router = useRouter();
  const [userDataExists, setUserDataExists] = useState<boolean>(false);

  useEffect(() => {
    const checkUserData = () => {
      const userData = localStorage.getItem("userData");
      setUserDataExists(!!userData);
    };

    checkUserData();

    const intervalId = setInterval(checkUserData, 1000);

    return () => clearInterval(intervalId);
  }, []);

  const handleMenuClick = async (e: { key: string }) => {
    if (e.key === "logout") {
      try {
        localStorage.removeItem("userData");
        const response = await fetch('/api/logout');
        if (response.ok) {
          message.success("Logged out successfully");
          router.push('/login');
        } else {
          message.error("An error occurred while logging out");
        }
      } catch (err) {
        message.error("An error occurred while logging out");
      }
    }
  };

  const items = userDataExists
    ? [
        { key: "myParticipant", label: <Link href={"/myParticipants"}>MyParticipants</Link> },
        { key: "Events", label: <Link href={"/events"}>Events</Link> },
        { key: "logout", label: <div className="logout__button">Log Out</div> }
      ]
    : [
        { key: "login", label: <Link href="/login">Login</Link> }
      ];

  return (
    <html lang="en">
      <body>
        <Layout style={{ minHeight: "100vh" }}>
          <Header>
            <Menu
              theme="dark"
              mode="horizontal"
              items={items}
              style={{ flex: 1, minWidth: 0, justifyContent: "end" }}
              onClick={handleMenuClick}
            />
          </Header>
          <Content style={{ padding: "0 48px" }}>
            {children}
          </Content>
          <Footer style={{ textAlign: "center" }}>EventsApp 2024. Created by John Gavr.</Footer>
        </Layout>
      </body>
    </html>
  );
};

export default RootLayout;
