"use client";

import { useSearchParams } from 'next/navigation';
import { useEffect, useState } from 'react';
import { apiFetch } from '../Services/apiClient';
import './event.css';

const EventPage = () => {
  const params = useSearchParams();
  const id = params.get('id');
  const [event, setEvent] = useState<eventObject | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (id) {
      fetchEventById(Number(id));
    }
  }, [id]);

  const fetchEventById = async (id: number) => {
    setLoading(true);
    setError(null);
    
    try {
      const response = await apiFetch(`/api/Events/event?Id=${id}`, {
        method: 'GET'
      });
  
      const data = await response.json();
  
      setEvent(data); 
    } catch (err) {
      setError('Failed to fetch event');
    } finally {
      setLoading(false);
    }
  };
  

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;
  if (!event) return <p>Event not found</p>;

  return (
    <div className="event">
      <h1 className="event__name">{event.name}</h1>
      <div className='event__info__wrapper'>
        <div className="event__info">
          <div className="card__wrapper">
            <h3>Category</h3>
            <p>{event.category}</p>
          </div>
          <div className="card__wrapper">
            <h3>Date</h3>
            <p>{new Date(event.eventDateTime).toLocaleDateString()}</p>
          </div>
          <div className="card__wrapper">
            <h3>Time</h3>
            <p>{new Date(event.eventDateTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</p>
          </div>
          <div className="card__wrapper">
            <h3>Location</h3>
            <p>{event.location}</p>
          </div>
          <div className="card__wrapper">
            <h3>Available spots</h3>
            <p>{event.maxParticipants - event.currentCountOfParticipants}</p>
          </div>
        </div>
        <img className="event__img" src="https://cdn-icons-png.flaticon.com/512/10701/10701484.png" alt="Event image" />
      </div>
      <p className="event__desc">{event.description}</p>
    </div>
  );
};

export default EventPage;
