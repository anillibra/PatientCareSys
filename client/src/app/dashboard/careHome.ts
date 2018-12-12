export class CareHome {
    public constructor(init?: Partial<CareHome>) {
      Object.assign(this, init);
    }
    id: number;
    name: string;
    category: string;
    location: string;
    address: string;
    isActive: boolean;
    createdOn: Date;
    updatedOn: Date;
    createdBy: number;
    updatedBy: number;

  }
