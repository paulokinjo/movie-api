// src/utils/auth.ts

const safeLocalStorage = {
  getItem: (key: string): string | null => {
    try {
      return localStorage.getItem(key);
    } catch (e) {
      console.error('localStorage getItem error', e);
      return null;
    }
  },
  setItem: (key: string, value: string): void => {
    try {
      localStorage.setItem(key, value);
    } catch (e) {
      console.error('localStorage setItem error', e);
    }
  },
  removeItem: (key: string): void => {
    try {
      localStorage.removeItem(key);
    } catch (e) {
      console.error('localStorage removeItem error', e);
    }
  }
};

export const getAuthToken = (): string | null => {
  if (typeof window !== 'undefined') {
    return safeLocalStorage.getItem('auth-token');
  }
  return null;
};

export const setAuthToken = (token: string): void => {
  if (typeof window !== 'undefined') {
    safeLocalStorage.setItem('auth-token', token);
  }
};

export const removeAuthToken = (): void => {
  if (typeof window !== 'undefined') {
    safeLocalStorage.removeItem('auth-token');
  }
};
