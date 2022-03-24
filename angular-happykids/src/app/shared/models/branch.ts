export interface Branch {
    id: number;
    street: string;
    city: string;
    description: string;
    latitude: number;
    longitude: number;
    workingHours: string;
    email: string;
    phone: string;
    country: string;
}

export interface BranchCreateEdit {
    id: number;
    street: string;
    city: string;
    description: string;
    latitude: number;
    longitude: number;
    workingHours: string;
    email: string;
    phone: string;
    countryId: number;
}
