export interface categoryResponse {
  id: string;
  name: string;
  description: string;
  isActive: boolean;
}

export interface categoryRequest {
  name:string;
  description:string
}