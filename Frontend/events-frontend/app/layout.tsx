"use client";
import { Content, Footer, Header } from "antd/es/layout/layout";
import "./globals.css";
import { Layout, Menu} from "antd";

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
            <Menu theme="dark" mode="horizontal" style={{ flex: 1, minWidth: 0, justifyContent: "end" }}>
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
