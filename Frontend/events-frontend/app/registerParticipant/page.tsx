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
  const [registrationError, setRegistrationError] = useState<string | null>(null);
  const [registrationSuccess, setRegistrationSuccess] = useState<string | null>(null);
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
      const response = await apiFetch('/api/UserData', {
        method: 'GET',
      });
      if (response.ok) {
        const serverData = await response.json();
        if (serverData.id === parsedLocalUserData.id) {
          setIsAuthorized(true); // User is authorized
        } else {
          localStorage.removeItem('userData');
          router.push('/login');
        }
      } else {
        router.push('/login');
      }
    } catch (err) {
      console.error("Authorization error:", err);
      router.push('/login');
    }
  };

  const handleRegistrationSubmit = async (values: any) => {
    setRegistrationError(null);
    setRegistrationSuccess(null);

    if (!values.firstName || !values.lastName || !values.dateOfBirth) {
      setRegistrationError('Please fill in all fields');
      return;
    }

    const requestData = {
      eventId: Number(eventId),
      firstName: values.firstName,
      lastName: values.lastName,
      dateOfBirth: `${values.dateOfBirth}T00:00:00`, 
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
        setRegistrationSuccess('Registration successful!');
        notification.success({
          message: 'Success',
          description: 'Registration successful!',
          placement: 'topRight',
        });
      } else {
        setRegistrationError('Failed to register');
        notification.error({
          message: 'Error',
          description: 'Failed to register',
          placement: 'topRight',
        });
      }
    } catch (err) {
      setRegistrationError('An error occurred during registration');
      notification.error({
        message: 'Error',
        description: 'An error occurred during registration',
        placement: 'topRight',
      });
    }
  };

  const handleBackButtonClick = () => {
    router.back();
  };

  if (!isAuthorized) {
    return <p>Loading...</p>;
  }

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
