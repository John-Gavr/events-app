import { Input, Modal, notification } from "antd";
import TextArea from "antd/es/input/TextArea";
import { useEffect, useState } from "react";

interface Props {
  mode: Mode;
  values: eventObject;
  isModalOpen: boolean;
  handleCancel: () => void;
  handleCreate: (request: eventRequest) => void;
  handleUpdate: (id: number, request: eventRequest) => void;
}

export enum Mode {
  Create,
  Edit,
}

export interface eventRequest {
  name: string;
  description: string;
  eventDateTime: string | null;
  location: string;
  category: string;
  maxParticipants: number;
  image: string;
}

export const CreateUpdateEvent = ({
  mode,
  values,
  isModalOpen,
  handleCancel,
  handleCreate,
  handleUpdate,
}: Props) => {
  const [name, setName] = useState<string>("");
  const [description, setDesc] = useState<string>("");
  const [location, setLocation] = useState<string>("");
  const [category, setCategory] = useState<string>("");
  const [maxParticipants, setMaxParticipants] = useState<number>(1);
  const [image, setImage] = useState<string>("");
  const [eventDateTime, setDateTime] = useState<string>("");

  const [imageFile, setImageFile] = useState<File | null>(null);
  const [date, setDate] = useState<string>("");
  const [time, setTime] = useState<string>("");

  const [error, setError] = useState<string | null>(null);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files ? event.target.files[0] : null;
    if (file) {
      setImageFile(file);
      const reader = new FileReader();
      reader.onloadend = () => {
        if (reader.result) {
          const base64String = (reader.result as string).split(',')[1]; 
          setImage(base64String);
        }
      };
      reader.readAsDataURL(file);
    }
  };

  const handleDateChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setDate(event.target.value);
  };

  const handleTimeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setTime(event.target.value);
  };

  useEffect(() => {
    if (values) {
      setName(values.name || "");
      setDesc(values.description || "");
      setCategory(values.category || "");
      setLocation(values.location || "");
      setMaxParticipants(values.maxParticipants || 1);
      setImage(values.image || "");

      if (values.eventDateTime) {
        const eventDate = new Date(values.eventDateTime);
        if (!isNaN(eventDate.getTime())) {
          setDate(eventDate.toISOString().split('T')[0]); 
          setTime(eventDate.toISOString().split('T')[1].substring(0, 5));
          setDateTime(eventDate.toISOString());
        } else {
          setDate("");
          setTime("");
          setDateTime("");
        }
      } else {
        setDate("");
        setTime("");
        setDateTime("");
      }
    }
  }, [values]);

  const handleOnOk = async () => {
    const fullDateTime = `${date}T${time}:00`;
    const eventRequest: eventRequest = {
      name,
      description,
      eventDateTime: fullDateTime || null,
      location,
      category,
      maxParticipants,
      image,
    };

    try {
      if (mode === Mode.Create) {
        await handleCreate(eventRequest);
        notification.success({
          message: 'Success',
          description: 'Event created successfully!',
          placement: 'topRight',
        });
      } else {
        await handleUpdate(values.id, eventRequest);
        notification.success({
          message: 'Success',
          description: 'Event updated successfully!',
          placement: 'topRight',
        });
      }
      handleCancel();
    } catch (err) {
      setError('An error occurred while saving the event. Please try again.');
      notification.error({
        message: 'Error',
        description: 'An error occurred while saving the event. Please try again.',
        placement: 'topRight',
      });
    }
  };

  return (
    <Modal
      title={mode === Mode.Create ? "Create Event" : "Edit Event"}
      open={isModalOpen}
      onOk={handleOnOk}
      onCancel={handleCancel}
      cancelText="Discard"
      okText={mode === Mode.Create ? "Create" : "Update"}
    >
      <Input
        value={name}
        onChange={(e) => setName(e.target.value)}
        placeholder="Event Title"
        style={{ marginBottom: '16px' }}
      />
      <Input
        value={location}
        onChange={(e) => setLocation(e.target.value)}
        placeholder="Location"
        style={{ marginBottom: '16px' }}
      />
      <Input
        value={category}
        onChange={(e) => setCategory(e.target.value)}
        placeholder="Category"
        style={{ marginBottom: '16px' }}
      />
      <Input
        value={maxParticipants}
        type="number"
        onChange={(e) => setMaxParticipants(Number(e.target.value))}
        placeholder="Max Participants"
        style={{ marginBottom: '16px' }}
      />
      <Input
        type="date"
        onChange={handleDateChange}
        value={date}
        style={{ marginBottom: '16px' }}
      />
      <Input
        type="time"
        onChange={handleTimeChange}
        value={time}
        style={{ marginBottom: '16px' }}
      />
      <Input
        type="file"
        accept="image/*"
        onChange={handleFileChange}
        style={{ marginBottom: '16px' }}
      />
      <TextArea
        value={description}
        onChange={(e) => setDesc(e.target.value)}
        placeholder="Event Description"
        style={{ marginBottom: '16px' }}
      />
      {error && <p style={{ color: 'red' }}>{error}</p>}
    </Modal>
  );
};
