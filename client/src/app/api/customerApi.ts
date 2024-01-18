import { CustomerModelResult, CustomersModelResult } from '../types/customers';
import { fetcher } from './fetcher';

export const getCustomer = async (
  customerId: number
): Promise<CustomerModelResult> =>
  await fetcher(`Customer/Single?userId=1&customerId=${customerId}`);

export const getCustomers = async (): Promise<CustomersModelResult> =>
  await fetcher('Customer/List?userId=1');
