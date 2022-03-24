import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LocationsService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getPureLocations() {
    return this.http.get<Location[]>(this.baseUrl + 'branches/locations');
  }
}
