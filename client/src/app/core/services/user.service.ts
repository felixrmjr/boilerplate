import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";

import { User } from "../models/user";
import { environment } from "src/environments/environment";
import { Observable } from "rxjs";

@Injectable({ providedIn: "root" })
export class UserService {
	private headers: { headers: HttpHeaders };
	private api: string;

	user: User;

	constructor(private http: HttpClient) {
		this.headers = { headers: new HttpHeaders({ "Content-Type": "application/json" }) };
		this.api = environment.backendurl + "/user/";
	}

    get(): Observable<any> {
        return this.http.get(this.api);
    }

    getById(id: string): Observable<any> {
        return this.http.get(this.api + id);
    }

    put(id: string, user: any): Observable<any> {
        return this.http.put(this.api + id, user, this.headers);
    }

    delete(id: string): Observable<any> {
        return this.http.delete(this.api + id);
    }
}
