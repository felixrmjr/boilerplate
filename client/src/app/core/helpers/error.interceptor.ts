import { Injectable } from "@angular/core";
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

import { AuthenticationService } from "../services/auth.service";
import { UserLogoutDTO } from "../models/dto/user.logout";
import { Router } from "@angular/router";

import Swal from "sweetalert2";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
	constructor(private router: Router, private authenticationService: AuthenticationService) {}

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		return next.handle(request).pipe(
			catchError((err) => {
				if (err.status === 401) {
					location.reload();

					let userLogout = new UserLogoutDTO();
					const currentUser = this.authenticationService.currentUser();
					userLogout.accessToken = currentUser.accessToken;
					this.authenticationService.logout(userLogout).subscribe({
						complete: () => {},
						error: (ex) => {
							Swal.fire({
								title: "Error",
								text: "Houve um erro",
								icon: "error",
								background: "#262b3c", //https://stackoverflow.com/questions/40418804/access-sass-values-colors-from-variables-scss-in-typescript-angular2-ionic2
							});
						},
						next: () => {
							this.router.navigate(["/account/login"]);
						},
					});
				}

				const error = err.error.message || err.statusText;
				return throwError(error);
			})
		);
	}
}
