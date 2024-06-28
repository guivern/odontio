export type UserDetailDto = {
  id: number;
  username: string;
  role: string;
  workspaceId: number;
  isActive: boolean;
  workspaceName: string;
  roleName: string;
  isDoctor: boolean;
  email: string | null;
  firstName: string | null;
  lastName: string | null;
  photoUrl: string | null;
};

export type UpsertUserDto = {
  username: string | null;
  password: string | null;
  confirmPassword: string | null;
  roleId: number | null;
  workspaceId: number | null;
  isActive: boolean;
  isDoctor: boolean;
  email: string | null;
  firstName: string | null;
  lastName: string | null;
  photoUrl: string | null;
};

export type ResetPasswordDto = {
  password: string;
  confirmPassword: string;
};

export type UpdateProfileDto = {
  email: string | null;
  firstName: string | null;
  lastName: string | null;
  photoUrl: string | null;
};

export type UserProfileDto = {
  username: string | null;
  email: string | null;
  firstName: string | null;
  lastName: string | null;
  photoUrl: string | null;
};

export type ChangePasswordDto = {
  oldPassword: string | null;
  newPassword: string | null;
  confirmPassword: string | null;
};