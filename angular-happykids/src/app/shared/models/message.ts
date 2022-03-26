export interface ClientMessage {
    id: number;
    firstLastName: string;
    email: string;
    phone: string;
    messageContent: string;
    isReplied: boolean;
    sendingDate: Date;
}

export interface MessageCreate {
    firstLastName: string;
    email: string;
    phone: string;
    messageContent: string;
}

export class MessageEdit {
    id: number;
    firstLastName: string;
    email: string;
    phone: string;
    messageContent: string;
    sendingDate: Date;
    isReplied: boolean;
}
