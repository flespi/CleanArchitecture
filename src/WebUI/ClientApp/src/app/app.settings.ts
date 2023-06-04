import { InjectionToken } from "@angular/core";

export const APP_SETTINGS = new InjectionToken<AppSettings>('APP_SETTINGS');

export type AppSettings = {
  apiBaseUrl: string,
  auth: {
    issuer: string,
    clientId: string
  },
};
