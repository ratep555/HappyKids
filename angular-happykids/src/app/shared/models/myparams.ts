import { User } from './user';

export class MyParams {
    orderStatusId = 0;
    manufacturerId = 0;
    locationId = 0;
    sort = 'something';
    tagId = 0;
    categoryId = 0;
    query: string;
    page = 1;
    pageCount = 12;
}

export class UserParams {
    orderStatusId = 0;
    manufacturerId = 0;
    roleId = 0;
    sort = 'something';
    tagId = 0;
    categoryId = 0;
    query: string;
    page = 1;
    pageCount = 12;
    displayName: string;

    constructor(user: User) {
        this.displayName = user.displayName;
   }
}
