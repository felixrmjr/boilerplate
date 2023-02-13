import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";

import { AuthenticationService } from "../../../core/services/auth.service";
import { environment } from "../../../../environments/environment";
import { first } from "rxjs/operators";
import { UserService } from "../../../core/services/user.service";
import { UserDTO } from "src/app/core/models/dto/user";

import Swal from "sweetalert2";

@Component({
	selector: "app-signup",
	templateUrl: "./signup.component.html",
	styleUrls: ["./signup.component.scss"],
})
export class SignupComponent implements OnInit {
	signupForm: FormGroup;
	submitted = false;
	error = "";
	successmsg = false;

	// set the currenr year
	year: number = new Date().getFullYear();

	constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private authenticationService: AuthenticationService, private userService: UserService) {}

	ngOnInit() {
		this.signupForm = this.formBuilder.group({
			username: ["", Validators.required],
			email: ["", [Validators.required, Validators.email]],
			password: ["", Validators.required],
		});
	}

	// convenience getter for easy access to form fields
	get f() {
		return this.signupForm.controls;
	}

	/**
	 * On submit form
	 */
	onSubmit() {
		this.submitted = true;

		// stop here if form is invalid
		if (this.signupForm.invalid) {
			return;
		} else {
			if (environment.defaultauth) {
				let user = new UserDTO();
				user.username = this.f.username.value;
				user.email = this.f.email.value;
				user.password = this.f.password.value;
				user.name = this.f.username.value;
				user.role = "user";
				user.profilePicture = "assets/images/users/avatar-1.jpg";
				this.authenticationService.register(user).subscribe({
					complete: () => {},
					error: (ex) => {
						Swal.fire({
							title: "Error",
							text: "Houve um erro ao se cadastrar",
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
