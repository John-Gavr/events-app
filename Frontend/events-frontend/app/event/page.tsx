"use client";

import { useSearchParams, useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';
import { apiFetch } from '../Services/apiClient';
import { Image, message } from 'antd';
import './event.css';

const EventPage = () => {
  const params = useSearchParams();
  const router = useRouter();
  const id = params.get('id');
  const [event, setEvent] = useState<eventObject | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [isRegistered, setIsRegistered] = useState<boolean>(false);
  const [loadingParticipants, setLoadingParticipants] = useState<boolean>(true);

  useEffect(() => {
    const userData = localStorage.getItem('userData');
    if (!userData) {
      router.push('/login');
      return;
    }

    const fetchEventDetails = async () => {
      if (id) {
        await fetchEventById(Number(id));
        await checkParticipantRegistration(Number(id));
      }
    };

    fetchEventDetails();
  }, [id, router]);

  const fetchEventById = async (id: number) => {
    setLoading(true);
    setError(null);

    try {
      const response = await apiFetch(`/api/Events/event?Id=${id}`, {
        method: 'GET',
      });

      const data = await response.json();
      setEvent(data);
    } catch (err) {
      setError('Failed to fetch event');
    } finally {
      setLoading(false);
    }
  };

  const checkParticipantRegistration = async (eventId: number) => {
    setLoadingParticipants(true);
    try {
      const localUserData = localStorage.getItem('userData');
      if (localUserData) {
        const { id: userId } = JSON.parse(localUserData);

        const response = await apiFetch(`/api/EventParticipant/event/participants?EventId=${eventId}`, {
          method: 'GET',
        });

        const participants = await response.json();
        const isUserRegistered = participants.some((participant: any) => participant.userId === userId);
        setIsRegistered(isUserRegistered);
      }
    } catch (err) {
      console.error("Failed to fetch participants", err);
      setError('Failed to fetch participants');
    } finally {
      setLoadingParticipants(false);
    }
  };

  const handleRegisterClick = () => {
    if (event) {
      router.push(`/registerParticipant?id=${event.id}`);
    }
  };

  const handleUnregisterClick = async () => {
    try {
      const response = await apiFetch(`/api/EventParticipant/event/unregister?EventId=${event?.id}`, {
        method: 'DELETE',
      });

      if (response.ok) {
        message.success('Successfully unregistered from the event');
        setIsRegistered(false);
      } else {
        message.error('Failed to unregister');
      }
    } catch (err) {
      message.error('Failed to unregister');
    }
  };

  if (loading || loadingParticipants) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;
  if (!event) return <p>Event not found</p>;

  const availableSpots = event.maxParticipants - event.currentCountOfParticipants;

  return (
    <div className="event">
      <h1 className="event__name">{event.name}</h1>
      <div className="event__info__wrapper">
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
            <p>{availableSpots}</p>
          </div>
        </div>

        <Image
          className="event__img"
          src={`data:image/jpeg;base64,${event.image}`}
          alt="Event image"
          fallback="https://cdn-icons-png.flaticon.com/512/10701/10701484.png"
        />
      </div>
      <p className="event__desc">{event.description}</p>

      {!isRegistered ? (
        <div className='participate__buttons'>
          {availableSpots > 0 ? (
            <button onClick={handleRegisterClick} className="button register__button">
              Register for this event
            </button>
          ) : (
            <div className="no__spots">
              No available spots for this event.
            </div>
          )}
        </div>
      ) : (
        <div className='participate__buttons'>
          <div className='already__participate'>You are already registered for this event.</div>
          <button onClick={handleUnregisterClick} className="button unregister__button">
            Unregister
          </button>
        </div>
      )}
    </div>
  );
};

export default EventPage;
