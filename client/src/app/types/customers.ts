import { BaseJsonResult } from './baseJsonResult';

export type CustomerDTO = {
  customerId: number;
  companyId: number;
  name: string;
};

export type CustomerModelResult = BaseJsonResult & {
  customer: CustomerDTO;
};

export type CustomersModelResult = BaseJsonResult & {
  customers: CustomerDTO[];
};
