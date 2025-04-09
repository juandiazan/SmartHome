import { HomeMemberPermission } from "./HomeMemberPermission";

export interface HomeMember {
    id:string;
    memberFullName:string;
    memberEmail:string;
    memberProfilePicture:string;    
    memberPermissions:HomeMemberPermission[];
    canReceiveNotifications:boolean;
}