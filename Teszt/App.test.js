import { render, screen, fireEvent } from '@testing-library/react';
import App from './App';
import {createMemoryHistory} from 'history'
import Login from './pages/Login';
import Contact from './pages/Contact';

test('Kapcsolato betöltődik', () => {
  expect(<Contact />);
});
test('Login betöltődik', () => {
  expect(<Login />);
});
test('App betöltődik', () => {
  expect(<App />);
});
test('Üdvözlő üzenet megjelenik', () => {
  render(<App />);
  const linkElement = screen.getByText(/Üdvözöljük az oldalunkon!/i);
  expect(linkElement).toBeInTheDocument();
});

test('A főoldalra vezet az oldal', () => {
  const history = createMemoryHistory();
  render(<App />);
  expect(history.location.pathname).toBe('/');
});
