import { apiClient } from "./client";
import { login, isStaffAuthenticated } from "./staff";

jest.mock("./client", () => ({
  apiClient: {
    get: jest.fn()
  }
}));

describe("staff.ts", () => {
  describe("login", () => {
    it("should return true when login is successful", async () => {
      (apiClient.get as jest.Mock).mockResolvedValue({ status: 200 });

      const result = await login("valid-access-code");

      expect(apiClient.get).toHaveBeenCalledWith("staff/login", {
        headers: { "X-Staff-Code": "valid-access-code" },
        credentials: "include"
      });
      expect(result).toBe(true);
    });

    it("should return false when login fails", async () => {
      (apiClient.get as jest.Mock).mockRejectedValue(new Error("Unauthorized"));

      const result = await login("invalid-access-code");

      expect(apiClient.get).toHaveBeenCalledWith("staff/login", {
        headers: { "X-Staff-Code": "invalid-access-code" },
        credentials: "include"
      });
      expect(result).toBe(false);
    });
  });

  describe("isStaffAuthenticated", () => {
    it("should return true when staff is authenticated", async () => {
      (apiClient.get as jest.Mock).mockResolvedValue({ status: 200 });

      const result = await isStaffAuthenticated();

      expect(apiClient.get).toHaveBeenCalledWith("staff/check", {
        credentials: "include"
      });
      expect(result).toBe(true);
    });

    it("should return false when staff is not authenticated", async () => {
      (apiClient.get as jest.Mock).mockRejectedValue(new Error("Unauthorized"));

      const result = await isStaffAuthenticated();

      expect(apiClient.get).toHaveBeenCalledWith("staff/check", {
        credentials: "include"
      });
      expect(result).toBe(false);
    });
  });
});
