import React from 'react';
import { Card, Image } from 'antd';
import Link from 'next/link';
import './eventsList.css';

interface EventListProps {
  events: eventObject[];
}

export const EventsListWithoutButtons: React.FC<EventListProps> = ({ events }) => {
  return (
    <div className="cards">
      {events.length === 0 ? (
        <p>No events available</p>
      ) : (
        events.map((event) => (
          <Card
            key={event.id}
            style={{ margin: '30px auto' }}
          >
            <Link
              className="event__header"
              href={{
                pathname: '/event',
                query: { id: event.id },
              }}
            >
              {event.name}
            </Link>
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
                  <p>{event.maxParticipants - event.currentCountOfParticipants}</p>
                </div>
              </div>
              <Image
                className="event__img"
                src={`data:image/jpeg;base64,${event.image}`}
                alt="Event image"
                fallback="https://cdn-icons-png.flaticon.com/512/10701/10701484.png"
              />
            </div>
          </Card>
        ))
      )}
    </div>
  );
};

export default EventsListWithoutButtons;
