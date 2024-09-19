"use client";
import { Content, Footer, Header } from "antd/es/layout/layout";
import "./globals.css";
import { Layout, Menu} from "antd";
import Link from "next/link";

const items = [
  {key : "myParticipant", label : <Link href={"/MyPartisipants"}>MyPartisipants</Link>},
  {key : "Events", label : <Link href={"/events"}>Events</Link>}
]

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <Layout style={{ minHeight: "100vh" }}>
          <Header>
            <Menu theme="dark" mode="horizontal" items={items} style={{ flex: 1, minWidth: 0, justifyContent: "end" }}>
            </Menu>
          </Header>
          <Content style={{ padding: "0 48px" }}>
            {children}
          </Content>
          <Footer style={{ textAlign: "center" }}>EventsApp 2024. Created by John Gavr.</Footer>
        </Layout>
      </body>
    </html>
  );
}
