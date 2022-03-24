import { Discount } from './discount';
import { KidActivity } from './kidactivity';

export interface BirthdayPackage {
    id: number;
    packageName: string;
    description: string;
    numberOfParticipants: number;
    price: number;
    additionalBillingPerParticipant: number;
    discountedAdditionalBillingPerParticipant: number;
    duration: number;
    picture: string;
    discountSum: number;
    discountedPrice?: number;
    hasDiscountsApplied?: boolean;
    kidActivities: KidActivity[];
}

export interface BirthdayPackageCreateEdit {
    id: number;
    packageName: string;
    description: string;
    numberOfParticipants: number;
    price: number;
    additionalBillingPerParticipant: number;
    duration: number;
    picture: File;
    discountsIds: number[];
    kidActivitiesIds: number[];
}

export interface BirthdayPackagePutGet {
    BirthdayPackage: BirthdayPackage;
    selectedKidActivities: KidActivity[];
    nonSelectedKidActivities: KidActivity[];
    selectedDiscounts: Discount[];
    nonSelectedDiscounts: Discount[];
}
