import { NextResponse } from 'next/server';

export async function GET() {
  console.log("asda");
  try {
    const response = NextResponse.redirect('http://localhost:3000/');
    response.cookies.set('.AspNetCore.Identity.Application', '', {
      maxAge: -1, 
      path: '/',
    });
    
    return response;
  } catch (error) {
    console.error('Logout API error:', error);
    return NextResponse.json({ error: 'Internal Server Error' }, { status: 500 });
  }
}
