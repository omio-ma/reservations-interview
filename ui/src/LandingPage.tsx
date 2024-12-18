import { Card, Dialog, Flex, Heading, Inset } from "@radix-ui/themes";
import { Link } from "@tanstack/react-router";
import { useState } from "react";
import { useIsStaffAuthenticated } from "./queries/useIsStaffAuthenticated";
import { LoginModal } from "./login/LoginModal";

export function LandingPage() {
  const [isLoginOpen, setLoginOpen] = useState(false);
  const { data: isAuthenticated, isLoading } = useIsStaffAuthenticated();
  return (
    <Flex direction="row" align="center" justify="center" gap="9" pt="9">
      <Card size="3" asChild variant="classic">
        <a href="#" onClick={() => setLoginOpen(true)}>
          <Inset side="top" pb="current">
            <img
              src="https://images.unsplash.com/photo-1550527882-b71dea5f8089?q=80&w=240&h=360&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
              alt="Key on wood board"
              style={{
                width: 240,
                height: 360
              }}
            />
          </Inset>
          <Heading align="center">Login</Heading>
        </a>
      </Card>
      <Card size="3" asChild variant="classic">
        <Link to="/reservations" preload="intent">
          <Inset clip="padding-box" side="top" pb="current">
            <img
              src="https://images.unsplash.com/photo-1531576788337-610fa9c67107?q=80&w=240&h=360&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
              alt="Clean Bed"
              style={{
                width: 240,
                height: 360
              }}
            />
          </Inset>
          <Heading align="center">Reserve</Heading>
        </Link>
      </Card>
      {isLoading ? (
        <Heading align="center">Checking...</Heading>
      ) : (
        isAuthenticated && (
          <Card size="3" asChild variant="classic">
            <Link to="/staff-page" preload="intent">
              <Inset clip="padding-box" side="top" pb="current">
                <img
                  src="https://images.unsplash.com/photo-1531576788337-610fa9c67107?q=80&w=240&h=360&auto=format&fit=crop"
                  alt="Clean Bed"
                  style={{
                    width: 240,
                    height: 360
                  }}
                />
              </Inset>
              <Heading align="center">Staff Page</Heading>
            </Link>
          </Card>
        )
      )}
      <Dialog.Root open={isLoginOpen} onOpenChange={setLoginOpen}>
        <LoginModal onClose={() => setLoginOpen(false)} />
      </Dialog.Root>
    </Flex>
  );
}
