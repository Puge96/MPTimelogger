const BASE_URL = 'http://localhost:3001/api/';
type httpVerb = 'GET' | 'POST' | 'PUT' | 'DELETE';

export const fetcher = async (
  url: string,
  method: httpVerb = 'GET',
  body?: object
) => {
  const result = await fetch(BASE_URL + url, {
    method,
    body: body === null ? null : JSON.stringify(body),
    headers: { 'Content-Type': 'application/json' }
  });

  // Error handling
  if (!result.ok) {
    try {
      // Attempt to parse the response as JSON.
      return await result.json();
    } catch (e) {
      return Promise.resolve({
        isValid: false,
        errors: ['An unexpected error occured:' + ' ' + result.statusText]
      });
    }
  }

  return result.json();
};
