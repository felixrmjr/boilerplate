import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

import { AuthenticationService } from "../../../core/services/auth.service";

import { ActivatedRoute, Router } from "@angular/router";

import { environment } from "../../../../environments/environment";
import { UserLoginDTO } from "src/app/core/models/dto/user.login";

import Swal from "sweetalert2";
@Component({
	selector: "app-login",
	templateUrl: "./login.component.html",
	styleUrls: ["./login.component.scss"],
})

/**
 * Login component
 */
export class LoginComponent implements OnInit {
	loginForm: FormGroup;
	submitted = false;
	error = "";
	returnUrl: string;

	// set the currenr year
	year: number = new Date().getFullYear();

	// tslint:disable-next-line: max-line-length
	constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private authenticationService: AuthenticationService) {}

	ngOnInit() {
		const currentUser = this.authenticationService.currentUser();
		if (currentUser) {
			return this.router.navigate(["/dashboard"]);
		}

		this.loginForm = this.formBuilder.group({
			email: ["dalacorte@email.com", [Validators.required, Validators.email]],
			password: ["12345", [Validators.required]],
		});

		// reset login status
		// this.authenticationService.logout();
		// get return url from route parameters or default to '/'
		// tslint:disable-next-line: no-string-literal
		this.returnUrl = this.route.snapshot.queryParams["returnUrl"] || "/";
	}

	// convenience getter for easy access to form fields
	get f() {
		return this.loginForm.controls;
	}

	/**
	 * Form submit
	 */
	onSubmit() {
		this.submitted = true;

		// stop here if form is invalid
		if (this.loginForm.invalid) {
			return;
		} else {
			if (environment.defaultauth) {
				let userLogin = new UserLoginDTO();
				userLogin.email = this.f.email.value;
				userLogin.password = this.f.password.value;
				this.authenticationService.login(userLogin).subscribe({
					complete: () => {},
					error: (ex) => {
						Swal.fire({
							title: "Error",
							text: "Houve um erro ao logar",
							icon: "error",
							background: "#262b3c", //https://stackoverflow.com/questions/40418804/access-sass-values-colors-from-variables-scss-in-typescript-angular2-ionic2
						});
					},
					next: () => {
						this.router.navigate(["/dashboard"]);
					},
				});
			}
		}
	}
}
