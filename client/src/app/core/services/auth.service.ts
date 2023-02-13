import { UserLogoutDTO } from "./../models/dto/user.logout";
import { environment } from "./../../../environments/environment";
import { Observable } from "rxjs";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { User } from "../models/user";
import { StorageProxy } from "./storage-proxy.service";
import { tap } from "rxjs/operators";
import { UserLoginDTO } from "../models/dto/user.login";
import { UserDTO } from "../models/dto/user";
import { UserRefreshDTO } from "../models/dto/user.refresh";

@Injectable({ providedIn: "root" })
export class AuthenticationService {
	private headers: { headers: HttpHeaders };
	private api: string;

	user: User;

	constructor(private http: HttpClient) {
		this.headers = { headers: new HttpHeaders({ "Content-Type": "application/json" }) };
		this.api = environment.backendurl + "/auth/";
	}

	public currentUser(): User {
		return StorageProxy.get("user");
	}

	login(userLogin: UserLoginDTO): Observable<any> {
		return this.http.post(this.api + "login", userLogin, this.headers).pipe(tap((user) => StorageProxy.set("user", user)));
	}

	register(user: UserDTO): Observable<any> {
		return this.http.post(this.api + "registration", user, this.headers).pipe(tap((user) => StorageProxy.set("user", user)));
	}

	refresh(userRefresh: UserRefreshDTO): Observable<any> {
		return this.http.post(this.api + "refresh", userRefresh, this.headers).pipe(tap((user) => StorageProxy.set("user", user)));
	}

	resetPassword(email: string) {}

	logout(userLogout: UserLogoutDTO) {
		return this.http.post(this.api + "logout", userLogout, this.headers).pipe(tap((user) => StorageProxy.remove("user")));
	}
}
