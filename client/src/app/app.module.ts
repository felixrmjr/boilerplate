import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule } from "@angular/core";
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from "@angular/common/http";

import { environment } from "../environments/environment";

import { NgbNavModule, NgbAccordionModule, NgbTooltipModule, NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { CarouselModule } from "ngx-owl-carousel-o";

import { ExtrapagesModule } from "./extrapages/extrapages.module";

import { LayoutsModule } from "./layouts/layouts.module";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { TranslateModule, TranslateLoader } from "@ngx-translate/core";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";
import { ErrorInterceptor } from "./core/helpers/error.interceptor";
import { JwtInterceptor } from "./core/helpers/jwt.interceptor";

if (environment.defaultauth) {
	//   initFirebaseBackend(environment.firebaseConfig);
}

export function createTranslateLoader(http: HttpClient): any {
	return new TranslateHttpLoader(http, "assets/i18n/", ".json");
}

@NgModule({
	declarations: [AppComponent],
	imports: [
		BrowserModule,
		BrowserAnimationsModule,
		HttpClientModule,
		TranslateModule.forRoot({
			loader: {
				provide: TranslateLoader,
				useFactory: createTranslateLoader,
				deps: [HttpClient],
			},
		}),
		LayoutsModule,
		AppRoutingModule,
		ExtrapagesModule,
		CarouselModule,
		NgbAccordionModule,
		NgbNavModule,
		NgbTooltipModule,
		NgbModule,
	],
	bootstrap: [AppComponent],
	providers: [
		{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
		{ provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
	],
})
export class AppModule {}
