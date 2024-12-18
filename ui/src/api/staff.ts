import { apiClient } from "./client";

export async function login(accessCode: string): Promise<boolean> {
  try {
    await apiClient.get("staff/login", {
      headers: { "X-Staff-Code": accessCode },
      credentials: "include"
    });
    return true;
  } catch (error: any) {
    return false;
  }
}

export async function isStaffAuthenticated(): Promise<boolean> {
  try {
    const response = await apiClient.get("staff/check", {
      credentials: "include"
    });
    return response.status === 200;
  } catch {
    return false;
  }
}
