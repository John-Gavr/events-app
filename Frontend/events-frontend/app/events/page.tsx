"use client";

import { useEffect, useState, useCallback } from 'react';
import { useRouter } from 'next/navigation';
import { Pagination, Input, Button, DatePicker, Modal, message } from "antd";
import dayjs from 'dayjs';
import { EventList } from "../Components/EventsList";
import { CreateUpdateEvent, Mode, eventRequest } from '../Components/CreateUpdateEvent';
import { apiFetch } from '../Services/apiClient';
import './page.css';

export default function EventsPage() {
  const router = useRouter();

  const [events, setEvents] = useState<eventObject[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [userRole, setUserRole] = useState<string | null>(null);

  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(2);
  const [totalItems, setTotalItems] = useState(0);

  const [selectedDate, setSelectedDate] = useState<dayjs.Dayjs | null>(null);
  const [location, setLocation] = useState<string>('');
  const [category, setCategory] = useState<string>('');

  const [mode, setMode] = useState(Mode.Create);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const [values, setValues] = useState<eventObject>({
    id: 0,
    name: "",
    description: "",
    eventDateTime: "",
    location: "",
    currentCountOfParticipants: 0,
    category: "",
    maxParticipants: 1,
    image: "",
  });

  const fetchEvents = useCallback(async (page: number, pageSize: number) => {
    setLoading(true);
    setError(null);
    try {
      const params = new URLSearchParams();

      params.append('PageNumber', page.toString());
      params.append('PageSize', pageSize.toString());

      if (location) {
        params.append('Location', location);
      }
      if (category) {
        params.append('Category', category);
      }
      if (selectedDate) {
        params.append('Date', selectedDate.format('YYYY-MM-DD'));
      }

      const response = await apiFetch(`/api/Events/bycriteria?${params.toString()}`);

      if (response.ok) {
        const data = await response.json();
        setEvents(data.events);
        setTotalItems(data.totalCount);
      } else {
        const errorData = await response.json();
        showError('Failed to fetch events', errorData.message || 'An unexpected error occurred.');
      }
    } catch (err) {
      showError('Failed to fetch events', 'An unexpected error occurred.');
    } finally {
      setLoading(false);
    }
  }, [location, category, selectedDate]);

  useEffect(() => {
    const userData = localStorage.getItem('userData');
    if (!userData) {
      router.push("/login");
      return;
    }

    const parsedUserData = JSON.parse(userData);
    if (parsedUserData.roles && parsedUserData.roles.length > 0) {
      setUserRole(parsedUserData.roles[0]);
    }

    fetchEvents(currentPage, pageSize);
  }, [currentPage, pageSize, fetchEvents, router]);

  const handlePageChange = (page: number, pageSize: number) => {
    setCurrentPage(page);
    setPageSize(pageSize);
  };

  const applyFilters = () => {
    setCurrentPage(1);
    fetchEvents(1, pageSize);
  };

  const resetFilters = () => {
    setLocation('');
    setCategory('');
    setSelectedDate(null);
    setCurrentPage(1);
    fetchEvents(1, pageSize);
  };

  const openCreateModal = () => {
    setMode(Mode.Create);
    setValues({
      id: 0,
      name: "",
      description: "",
      eventDateTime: "",
      location: "",
      currentCountOfParticipants: 0,
      category: "",
      maxParticipants: 1,
      image: "",
    });
    setIsModalOpen(true);
  };

  const openEditModal = (event: eventObject) => {
    setMode(Mode.Edit);
    setValues(event);
    setIsModalOpen(true);
  };

  const handleCancel = () => {
    setIsModalOpen(false);
  };

  const handleCreate = async (request: eventRequest) => {
    try {
      const response = await apiFetch('/api/Events', {
        method: 'POST',
        body: JSON.stringify(request),
      });
      if (response.ok) {
        message.success('Event created successfully');
        setIsModalOpen(false);
        router.push("/events");
      } else {
        const errorData = await response.json();
        showError('Failed to create event', errorData.message || 'An unexpected error occurred.');
      }
    } catch (err) {
      showError('Failed to create event', 'An unexpected error occurred.');
    }
  };

  const handleUpdate = async (id: number, request: eventRequest) => {
    try {
      const response = await apiFetch(`/api/Events?id=${id}`, {
        method: 'PUT',
        body: JSON.stringify(request),
      });
      if (response.ok) {
        message.success('Event updated successfully');
        setIsModalOpen(false);
        fetchEvents(currentPage, pageSize);
      } else {
        const errorData = await response.json();
        showError('Failed to update event', errorData.message || 'An unexpected error occurred.');
      }
    } catch (err) {
      showError('Failed to update event', 'An unexpected error occurred.');
    }
  };

  const handleDelete = async (id: number) => {
    try {
      const response = await apiFetch(`/api/Events?id=${id}`, {
        method: 'DELETE',
      });
      if (response.ok) {
        message.success('Event deleted successfully');
        setEvents(prevEvents => prevEvents.filter(event => event.id !== id));
        setTotalItems(prevTotal => prevTotal - 1);
      } else {
        const errorData = await response.json();
        showError('Failed to delete event', errorData.message || 'An unexpected error occurred.');
      }
    } catch (err) {
      showError('Failed to delete event', 'An unexpected error occurred.');
    }
  };

  const showError = (title: string, description: string) => {
    Modal.error({
      title,
      content: description,
      centered: true,
      okText: 'OK'
    });
  };

  if (!userRole) {
    return <p>Loading...</p>;
  }

  return (
    <div>
      <div className='control__panel'>
        {userRole === 'Admin' && (
          <Button type="primary" onClick={openCreateModal}>
            Create Event
          </Button>
        )}

        <CreateUpdateEvent
          mode={mode}
          values={values}
          isModalOpen={isModalOpen}
          handleCancel={handleCancel}
          handleCreate={handleCreate}
          handleUpdate={handleUpdate}
        />
      </div>

      <div className="filters">
        <Input
          placeholder="Location"
          value={location}
          onChange={(e) => setLocation(e.target.value)}
          className="filter"
        />

        <Input
          placeholder="Category"
          value={category}
          onChange={(e) => setCategory(e.target.value)}
          className="filter"
        />

        <DatePicker
          onChange={(date) => setSelectedDate(date)}
          className="filter"
          placeholder="Select Date"
        />

        <Button type="primary" onClick={applyFilters}>
          Apply Filters
        </Button>
        <Button onClick={resetFilters}>
          Reset Filters
        </Button>
      </div>
      {loading ? (
        <p>Loading...</p>
      ) : (
        <div>
          <EventList
            events={events}
            openEditModal={openEditModal}
            handleDelete={handleDelete}
            isAdmin={userRole === 'Admin'}
          />
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
