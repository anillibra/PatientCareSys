import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CareHome } from './careHome';

const API_URL = 'http://localhost:51004/api/carehome/all';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
// 51004
  constructor(private http: HttpClient) { }

  getCategories(): Observable<CareHome[]> {
    return this.http.get<CareHome[]>(API_URL);
  }

}
