import { Component, OnInit, Output, EventEmitter, Inject } from "@angular/core";
import { Router } from "@angular/router";
import { DOCUMENT } from "@angular/common";
import { AuthenticationService } from "../../core/services/auth.service";
import { environment } from "../../../environments/environment";
import { CookieService } from "ngx-cookie-service";
import { LanguageService } from "../../core/services/language.service";
import { TranslateService } from "@ngx-translate/core";
import { User } from "src/app/core/models/user";
import { UserLogoutDTO } from "src/app/core/models/dto/user.logout";
import { JWTService } from "src/app/core/services/jwt.service";
import { UserRefreshDTO } from "src/app/core/models/dto/user.refresh";

import Swal from "sweetalert2";

@Component({
	selector: "app-topbar",
	templateUrl: "./topbar.component.html",
	styleUrls: ["./topbar.component.scss"],
})
export class TopbarComponent implements OnInit {
	element;
	cookieValue;
	flagvalue;
	countryName;
	valueset;

	user: User;
	profilePicture: string;

	constructor(@Inject(DOCUMENT) private document: any, private router: Router, private authenticationService: AuthenticationService, public languageService: LanguageService, public translate: TranslateService, public _cookiesService: CookieService, private JWTService: JWTService) {}

	listLang = [
		{ text: "Portuguese", flag: "assets/images/flags/brazil.jpg", lang: "pt" },
		{ text: "English", flag: "assets/images/flags/us.jpg", lang: "en" },
		{ text: "Spanish", flag: "assets/images/flags/spain.jpg", lang: "es" },
		{ text: "German", flag: "assets/images/flags/germany.jpg", lang: "de" },
		{ text: "Italian", flag: "assets/images/flags/italy.jpg", lang: "it" },
		{ text: "Russian", flag: "assets/images/flags/russia.jpg", lang: "ru" },
	];

	openMobileMenu: boolean;

	@Output() settingsButtonClicked = new EventEmitter();
	@Output() mobileMenuButtonClicked = new EventEmitter();

	ngOnInit() {
		this.openMobileMenu = false;
		this.element = document.documentElement;

		const user = this.authenticationService.currentUser();

		if (user) {
			this.user = user;
			this.verifyRefreshToken(this.user.accessToken);
			const match =
				/^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:[/?#]\S*)?$/i.test(
					user.profilePicture
				);
			if (match) {
				this.profilePicture = this.user.profilePicture;
			} else {
				this.profilePicture = "assets/images/users/avatar-1.jpg";
			}
		}

		this.cookieValue = this._cookiesService.get("lang");
		const val = this.listLang.filter((x) => x.lang === this.cookieValue);
		this.countryName = val.map((element) => element.text);
		if (val.length === 0) {
			if (this.flagvalue === undefined) {
				this.valueset = "assets/images/flags/us.jpg";
			}
		} else {
			this.flagvalue = val.map((element) => element.flag);
		}
	}

	verifyRefreshToken(refreshToken: string) {
		const expirationDate = this.JWTService.getTokenExpirationDate(refreshToken);

		if (expirationDate > new Date()) {
			if (Math.abs(Number(expirationDate) - Number(new Date())) / 36e5 <= 1) {
				let userRefresh = new UserRefreshDTO();
				userRefresh.refreshToken = refreshToken;
				this.authenticationService.refresh(userRefresh).subscribe({
					complete: () => {},
					error: (ex) => {
						console.log(ex);
					},
					next: () => {},
				});
			}
		} else {
			let userLogout = new UserLogoutDTO();
			userLogout.accessToken;
			this.authenticationService.logout(userLogout).subscribe({
				complete: () => {},
				error: (ex) => {
					console.log(ex);
				},
				next: () => {
					this.router.navigate(["/account/login"]);
				},
			});
		}
	}

	setLanguage(text: string, lang: string, flag: string) {
		this.countryName = text;
		this.flagvalue = flag;
		this.cookieValue = lang;
		this.languageService.setLanguage(lang);
	}

	/**
	 * Toggles the right sidebar
	 */
	toggleRightSidebar() {
		this.settingsButtonClicked.emit();
	}

	/**
	 * Toggle the menu bar when having mobile screen
	 */
	toggleMobileMenu(event: any) {
		event.preventDefault();
		this.mobileMenuButtonClicked.emit();
	}

	/**
	 * Logout the user
	 */
	logout() {
		Swal.fire({
			title: "Você deseja sair?",
			// text: 'Você será redirecionado para tela de login',
			icon: "warning",
			background: "#262b3c", //https://stackoverflow.com/questions/40418804/access-sass-values-colors-from-variables-scss-in-typescript-angular2-ionic2
			showCancelButton: true,
			confirmButtonColor: "#34c38f",
			cancelButtonColor: "#f46a6a",
			confirmButtonText: "Sim, sair!",
			cancelButtonText: "Não, continuar logado!",
		}).then((result) => {
			if (result.value) {
				if (environment.defaultauth) {
					let userLogout = new UserLogoutDTO();
					userLogout.accessToken = this.user.accessToken;
					this.authenticationService.logout(userLogout).subscribe({
						complete: () => {},
						error: (ex) => {
							Swal.fire({
								title: "Error",
								text: "Houve um erro ao sair",
								icon: "error",
								background: "#262b3c", //https://stackoverflow.com/questions/40418804/access-sass-values-colors-from-variables-scss-in-typescript-angular2-ionic2
							});
						},
						next: () => {
							this.router.navigate(["/account/login"]);
						},
					});
				}
			}
		});
	}

	/**
	 * Fullscreen method
	 */
	fullscreen() {
		document.body.classList.toggle("fullscreen-enable");
		if (!document.fullscreenElement && !this.element.mozFullScreenElement && !this.element.webkitFullscreenElement) {
			if (this.element.requestFullscreen) {
				this.element.requestFullscreen();
			} else if (this.element.mozRequestFullScreen) {
				/* Firefox */
				this.element.mozRequestFullScreen();
			} else if (this.element.webkitRequestFullscreen) {
				/* Chrome, Safari and Opera */
				this.element.webkitRequestFullscreen();
			} else if (this.element.msRequestFullscreen) {
				/* IE/Edge */
				this.element.msRequestFullscreen();
			}
		} else {
			if (this.document.exitFullscreen) {
				this.document.exitFullscreen();
			} else if (this.document.mozCancelFullScreen) {
				/* Firefox */
				this.document.mozCancelFullScreen();
			} else if (this.document.webkitExitFullscreen) {
				/* Chrome, Safari and Opera */
				this.document.webkitExitFullscreen();
			} else if (this.document.msExitFullscreen) {
				/* IE/Edge */
				this.document.msExitFullscreen();
			}
		}
	}
}
