export const StorageProxy = {
	getString: (prop: string): string => {
		return localStorage.getItem(prop);
	},
	get: <T extends object>(prop: string): T => {
		const data = localStorage.getItem(prop);
		try {
			return JSON.parse(data) as T;
		} catch {
			return null;
		}
	},
	set: (prop: string, value: any) => {
		localStorage.setItem(prop, typeof value === "object" ? JSON.stringify(value) : value);
	},
	remove: (prop: string) => {
		localStorage.removeItem(prop);
	},
};
