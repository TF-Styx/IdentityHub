import { CommonModule } from "@angular/common";
import { Component, inject, OnInit, signal, WritableSignal } from "@angular/core";
import { firstValueFrom } from "rxjs";
import { ProfileApi } from "./profile.api";
import { ProfileGeneralInfoResponse } from "../../entities/user/model/types";
import { ProfileGeneralInfoEditComponent } from "../../features/profile/general-info/edit/profile-general-info-edit.component";
import { ProfileGeneralInfoViewComponent } from "../../features/profile/general-info/view/profile-general-info-view.component";
import { ProfileAvatarEditComponent } from "../../features/profile/avatar/edit/profile-avatar-edit.component";
import { ProfileAvatarViewComponent } from "../../features/profile/avatar/view/profile-avatar-view.component";

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
  standalone: true,
  imports: [
    CommonModule, 
    ProfileGeneralInfoEditComponent,
    ProfileGeneralInfoViewComponent,
    ProfileAvatarEditComponent,
    ProfileAvatarViewComponent],
})

export class ProfilePage implements OnInit {
    private profileApi = inject(ProfileApi);

    isLoaded: WritableSignal<boolean> = signal(false);
    isEditMode: WritableSignal<boolean> = signal(false);
    isSubmitting: WritableSignal<boolean> = signal(false);
    userData: WritableSignal<Partial<ProfileGeneralInfoResponse>> = signal({});
    avatarUrl: WritableSignal<string | null> = signal<string | null>(null);
    message: WritableSignal<string | null> = signal<string | null>(null);
    originalUserData: WritableSignal<Partial<ProfileGeneralInfoResponse>> = signal({});
  
    pendingAvatarFile: File | null = null;

    ngOnInit(): void {
        this.loadData();
    }

    private loadData(): void {
        this.profileApi.getProfileGeneralInfo().subscribe({
        next: (profile: any) => {
            this.userData.set(profile);
            this.avatarUrl.set(profile.avatar ?? null);
            this.isLoaded.set(true);
        },
        error: () => this.message.set('Ошибка загрузки профиля')
        });
    }

    onEdit(): void {
        const currentData = this.userData();
        this.originalUserData.set({ ...currentData });
        this.isEditMode.set(true);
    }

    onAvatarSelected(file: File): void {
        this.pendingAvatarFile = file;
    }

    async onSaveGeneralInfo(payload: { userName: string }): Promise<void> {
        this.isSubmitting.set(true);
        this.message.set(null);
        
        // 1. Обновляем имя на сервере
        try {
            // await firstValueFrom(this.profileApi.updateUserName({ userName: payload.userName }));
            this.profileApi.updateUserName({userName: payload.userName})
                .subscribe({
                    next: () => {
                        console.log('Изменение имени пользователя прошло успешно!')
                        this.userData.update(data => ({
                            data,
                            login: data.login,
                            userName: payload.userName,
                            email: data.email
                        }));
                    },
                    error: errors => console.log(errors)
                });
            
            // 2. Загружаем аватар, если выбран
            if (this.pendingAvatarFile) {
                const fromData = new FormData();
                fromData.append('File', this.pendingAvatarFile, this.pendingAvatarFile.name);

                await firstValueFrom(this.profileApi.uploadAvatar(fromData));
                
                this.avatarUrl.set(URL.createObjectURL(this.pendingAvatarFile));
                this.pendingAvatarFile = null;
            }

        this.message.set('Данные успешно сохранены');
        await this.loadData(); // перечитываем актуальные данные
        this.isEditMode.set(false);
        } catch (err) {
            this.message.set('Ошибка при сохранении');
        } finally {
            this.isSubmitting.set(false);
        }
    }

    onCancel(): void {
        const originalData = this.originalUserData();
        this.userData.set({ ...originalData });
        this.avatarUrl.set(originalData.avatar || null);
        this.pendingAvatarFile = null;
        this.message.set(null);
        this.isEditMode.set(false);
    }
}
