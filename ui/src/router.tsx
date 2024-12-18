import {
  createRootRoute,
  createRoute,
  createRouter
} from "@tanstack/react-router";
import { Layout } from "./Layout";
import { LandingPage } from "./LandingPage";
import { ReservationPage } from "./reservations/ReservationPage";
import { StaffPage } from "./staff/StaffPage";
import { isStaffAuthenticated } from "./api/staff";
import { redirect } from "@tanstack/react-router";

const rootRoute = createRootRoute({
  component: Layout
});

function getRootRoute() {
  return rootRoute;
}

const ROUTES = [
  createRoute({
    path: "/",
    getParentRoute: getRootRoute,
    component: LandingPage
  }),
  createRoute({
    path: "/reservations",
    getParentRoute: getRootRoute,
    component: ReservationPage
  }),
  createRoute({
    path: "/staff-page",
    getParentRoute: getRootRoute,
    component: StaffPage,
    beforeLoad: async () => {
      const isAuthenticated = await isStaffAuthenticated();
      if (!isAuthenticated) {
        throw redirect({ to: "/" });
      }
    }
  })
];

const routeTree = rootRoute.addChildren(ROUTES);

export const router = createRouter({ routeTree });
