import ky from "ky";

export const apiClient = ky.create({
  prefixUrl: "api",
  headers: {
    "Content-Type": "application/json"
  }
});

export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  error?: string;
}
