import { Injectable } from "@angular/core";
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";

import { AuthenticationService } from "../services/auth.service";

import { environment } from "../../../environments/environment";

@Injectable({ providedIn: "root" })
export class AuthGuard implements CanActivate {
	constructor(private router: Router, private authenticationService: AuthenticationService) {}

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
		if (environment.defaultauth) {
			const currentUser = this.authenticationService.currentUser();
			if (currentUser) {
				return true;
			}
		}

		this.router.navigate(["/account/login"], { queryParams: { returnUrl: state.url } });
		return false;
	}
}
