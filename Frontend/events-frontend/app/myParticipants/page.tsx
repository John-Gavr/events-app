"use client";

import { apiFetch } from '../Services/apiClient';
import { Pagination, message } from "antd";
import { useEffect, useState, useCallback } from "react";
import { EventsListWithoutButtons } from '../Components/EventsListWithoutButtons';
import './page.css';
import { useRouter } from 'next/navigation';

export default function EventsPage() {
  const router = useRouter();

  const [events, setEvents] = useState<eventObject[]>([]);
  const [loading, setLoading] = useState(true);
  const [userId, setUserId] = useState<string | null>(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(2);
  const [totalItems, setTotalItems] = useState(0);

  const fetchUserEvents = useCallback(async (page: number, pageSize: number) => {
    setLoading(true);
    try {
      if (userId) {
        const response = await apiFetch(`/api/Events/userEvents?UserId=${userId}&PageNumber=${page}&PageSize=${pageSize}`);

        if (response.ok) {
          const data = await response.json();
          setEvents(data.events);
          setTotalItems(data.totalCount);
        } else {
          message.error("Failed to fetch user events");
        }
      } else {
        message.error("User ID not found");
        router.push("/login");
      }
    } catch (err) {
      message.error("Failed to fetch user events");
    } finally {
      setLoading(false);
    }
  }, [userId, router]);

  useEffect(() => {
    const userData = localStorage.getItem('userData');
    if (userData) {
      const parsedUserData = JSON.parse(userData);
      if (parsedUserData.id) {
        setUserId(parsedUserData.id);
      } else {
        message.error("User ID is missing in userData");
        router.push("/login");
      }
    } else {
      router.push("/login");
    }
  }, [router]);

  useEffect(() => {
    if (userId) {
      fetchUserEvents(currentPage, pageSize);
    }
  }, [userId, currentPage, pageSize, fetchUserEvents]);

  const handlePageChange = (page: number, pageSize: number) => {
    setCurrentPage(page);
    setPageSize(pageSize);
  };

  return (
    <div>
      {loading ? (
        <p>Loading...</p>
      ) : (
        <div>
          <EventsListWithoutButtons events={events} />
          <Pagination
            current={currentPage}
            pageSize={pageSize}
            total={totalItems}
            onChange={handlePageChange}
          />
        </div>
      )}
    </div>
  );
}
