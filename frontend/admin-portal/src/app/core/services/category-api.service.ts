import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { categoryResponse, categoryRequest } from '../types/category';
import {Observable} from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class CategoryApiService {

  private readonly apiUrl =   `${environment.apiUrl}/Categories`;
  constructor(private http:HttpClient) { }

  getAll():Observable<categoryResponse[]>{
    return this.http.get<categoryResponse[]>(`${this.apiUrl}`);
  }

  getById(id:string):Observable<categoryResponse>{
   return this.http.get<categoryResponse>(`${this.apiUrl}/${id}`);
  }

  remove(id:string):Observable<void>{
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
  }

  create(request:categoryRequest):Observable<categoryResponse>{
    return this.http.post<categoryResponse>(`${this.apiUrl}/create`, request);
  }

  update(id:string,request:categoryRequest):Observable<categoryResponse>{
    return this.http.put<categoryResponse>(`${this.apiUrl}/${id}`, request) 
  }
  
}
