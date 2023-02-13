import { UserService } from './../../../core/services/user.service';
import { AuthenticationService } from './../../../core/services/auth.service';
import { Component, OnInit } from "@angular/core";
import { User } from "src/app/core/models/user";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import Swal from "sweetalert2";

@Component({
	selector: "app-profile",
	templateUrl: "./profile.component.html",
	styleUrls: ["./profile.component.scss"],
})
export class ProfileComponent implements OnInit {

    breadCrumbItems: Array<{}>;
    
    loginForm: FormGroup;
    user: User;
    profilePicture: string;
    createdDate: string;

    isChangingPassword: boolean = false;
    changePassword1: string;
    changePassword2: string;

    submitted: boolean = false;
    error: string = '';
    
    constructor(private formBuilder: FormBuilder, private authenticationService: AuthenticationService, private userService: UserService) {}

	ngOnInit() {
        this.breadCrumbItems = [{ label: 'Profile', active: true }];

        this.user = this.authenticationService.currentUser();;
        const date = new Date(this.user.createdDate);
        const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: 'numeric' };
        this.createdDate = date.toLocaleDateString("pt-BR", options);

        this.loginForm = this.formBuilder.group({
            email: [this.user.email, [Validators.required, Validators.email]],
            name: [this.user.name, [Validators.required]],
            password: [this.user.password, [Validators.required]],
            id: [this.user.id, [Validators.required]],
            username: [this.user.username, [Validators.required]],
            changePassword1: [this.changePassword1],
            changePassword2: [this.changePassword2]
        });

        this.loginForm.controls['password'].disable();
        this.loginForm.controls['id'].disable();
        this.loginForm.controls['username'].disable();
    }

    changePassword() {
        if (!this.isChangingPassword) {
            this.f.changePassword1.reset();
            this.f.changePassword2.reset();
        }
    }

    get f() {
        return this.loginForm.controls;
    }

    onSubmit() {
        this.submitted = true;

        if (this.loginForm.invalid) {
            return;
        }

        if (this.f.changePassword1.value !== this.f.changePassword2.value) {
            Swal.fire({
                title: "Error",
                text: "As duas senhas precisam ser iguais",
                icon: "error",
                background: "#262b3c", //https://stackoverflow.com/questions/40418804/access-sass-values-colors-from-variables-scss-in-typescript-angular2-ionic2
            });

            return;
        }

        this.user.email = this.f.email.value;
        this.user.name = this.f.name.value;
        this.user.email = this.f.email.value;
        
        if (this.isChangingPassword)
            this.user.password = this.changePassword1; 

        this.userService.put(this.user.id, this.user).subscribe({
            complete: () => { },
            error: (ex) => {
                Swal.fire({
                    title: "Error",
                    text: "Houve um erro ao salvar",
                    icon: "error",
                    background: "#262b3c", //https://stackoverflow.com/questions/40418804/access-sass-values-colors-from-variables-scss-in-typescript-angular2-ionic2
                });
            },
            next: () => { 
                Swal.fire({
                    title: "Sucesso",
                    text: "Informações atualizadas com sucesso",
                    icon: "success",
                    background: "#262b3c", //https://stackoverflow.com/questions/40418804/access-sass-values-colors-from-variables-scss-in-typescript-angular2-ionic2
                });
            },
        });
    }
}
