"use client";

import { useRouter, useSearchParams } from 'next/navigation';
import { useEffect, useState } from 'react';
import { apiFetch } from '../Services/apiClient';
import { Form, Input, Button, notification } from 'antd';

const RegisterPage = () => {
  const router = useRouter();
  const params = useSearchParams();
  const eventId = params.get('id');
  const [email, setEmail] = useState<string>('');
  const [isAuthorized, setIsAuthorized] = useState<boolean>(false);

  useEffect(() => {
    checkAuthorization();
  }, []);

  const checkAuthorization = async () => {
    const localUserData = localStorage.getItem('userData');
    if (!localUserData) {
      router.push('/login');
      return;
    }

    const parsedLocalUserData = JSON.parse(localUserData);
    setEmail(parsedLocalUserData.email);

    try {
      const response = await apiFetch('/api/UserData');
      if (response.ok) {
        const serverData = await response.json();
        if (serverData.id === parsedLocalUserData.id) {
          setIsAuthorized(true);
        } else {
          handleAuthorizationError();
        }
      } else {
        handleAuthorizationError();
      }
    } catch (err) {
      console.error("Authorization error:", err);
      handleAuthorizationError();
    }
  };

  const handleAuthorizationError = () => {
    localStorage.removeItem('userData');
    router.push('/login');
  };

  const handleRegistrationSubmit = async (values: any) => {
    if (!eventId) {
      notification.error({
        message: 'Error',
        description: 'Event ID is missing.',
        placement: 'topRight',
      });
      return;
    }

    const { firstName, lastName, dateOfBirth } = values;
    if (!firstName || !lastName || !dateOfBirth) {
      notification.error({
        message: 'Error',
        description: 'Please fill in all fields.',
        placement: 'topRight',
      });
      return;
    }

    const requestData = {
      eventId: Number(eventId),
      firstName,
      lastName,
      dateOfBirth: `${dateOfBirth}T00:00:00`,
      registrationDate: new Date().toISOString(),
      email,
    };

    try {
      const response = await apiFetch('/api/EventParticipant/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(requestData),
      });

      if (response.ok) {
        notification.success({
          message: 'Success',
          description: 'Registration successful!',
          placement: 'topRight',
        });
        router.push('/events');
      } else {
        const errorData = await response.json();
        notification.error({
          message: 'Error',
          description: errorData.message || 'Failed to register.',
          placement: 'topRight',
        });
      }
    } catch (err) {
      console.error('Registration error:', err);
      notification.error({
        message: 'Error',
        description: 'An error occurred during registration.',
        placement: 'topRight',
      });
    }
  };

  const handleBackButtonClick = () => {
    router.back();
  };

  if (!isAuthorized) return <p>Loading...</p>;

  return (
    <div className="registration-page">
      <h2>Register for the event</h2>
      <Form
        onFinish={handleRegistrationSubmit}
        layout="vertical"
      >
        <Form.Item
          label="First Name"
          name="firstName"
          rules={[{ required: true, message: 'Please enter your first name' }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Last Name"
          name="lastName"
          rules={[{ required: true, message: 'Please enter your last name' }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Date of Birth"
          name="dateOfBirth"
          rules={[{ required: true, message: 'Please select your date of birth' }]}
        >
          <Input type="date" />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit">
            Register
          </Button>
          <Button
            type="default"
            onClick={handleBackButtonClick}
            style={{ marginLeft: '10px' }}
          >
            Back
          </Button>
        </Form.Item>
      </Form>
    </div>
  );
};

export default RegisterPage;
