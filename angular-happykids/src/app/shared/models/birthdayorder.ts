export interface ClientBirthdayOrder {
    id: number;
    branch: string;
    birthdayPackage: string;
    orderStatus: string;
    clientName: string;
    birthdayGirlBoyName: string;
    contactPhone: string;
    contactEmail: string;
    numberOfGuests: number;
    remarks?: any;
    birthdayNo: number;
    price: number;
    startDateAndTime: Date;
    endDateAndTime: Date;
    orderStatusId?: number;
}

export interface BirthdayOrderCreate {
    branchId: number;
    birthdayPackageId: number;
    clientName: string;
    birthdayGirlBoyName: string;
    contactEmail: string;
    contactPhone: string;
    numberOfGuests: number;
    birthdayNo: number;
    startDateAndTime: Date;
}
