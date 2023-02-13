export class UserDTO {
	id: string;
	username: string;
	password: string;
	email: string;
	name: string;
	profilePicture: string | null;
	role: string;
	accessToken: string | null;
	refreshToken: string | null;
	isActive: boolean | null;
	language: string | null;
	createdDate: string | null;
}
