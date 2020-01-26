<template>

  <v-menu
    v-model="model.model"
    :close-on-content-click="false"
    :activator="model.element"
    offset-x
  >
    <v-card
      v-if="getShift"
      min-width="350px"
    >
      <v-toolbar
        :color="model.event.color"
        prominent
      >
        <v-toolbar-title>
          {{ getShift.getTimespan(userModule.getUser) }}
        </v-toolbar-title>
        <v-spacer></v-spacer>
        <ShiftAssignmentDialog :shift="getShift" />
        <UpdateShiftDialog
          v-if="canEdit"
          :shift="getShift"
        />
        <v-btn
          icon
          :title="$t('core.close')"
          @click="model.model = false"
        >
          <v-icon>close</v-icon>
        </v-btn>
      </v-toolbar>
      <v-card-actions
        v-if="myApplication"
        class="accent"
      >
        <span
          class="ml-2"
          v-t="'shift.application.youApplied'"
        />
        <v-spacer />
        <DeleteApplicationDialog :application="myApplication" />
      </v-card-actions>
      <v-card-actions v-else>
        <v-spacer />
        <CreateApplicationDialog :shift="getShift" />
      </v-card-actions>
      <v-list v-if="getShift.applications.length > 0">
        <v-subheader>
          {{ $tc('shift.application.applicants', 2) }}
          <v-spacer />
          <v-btn
            icon
            @click="showApplicants = !showApplicants"
          >
            <v-icon v-if="showApplicants">keyboard_arrow_up</v-icon>
            <v-icon v-else>keyboard_arrow_down</v-icon>
          </v-btn>
        </v-subheader>
        <div v-if="showApplicants">
          <v-list-item
            v-for="(application, index) in getShift.getApplications"
            :key="index"
            @click=""
          >
            <v-list-item-avatar>
              <v-icon color="accent">emoji_people</v-icon>
            </v-list-item-avatar>
            <v-list-item-content>
              <v-list-item-title>{{ application.person.getFullName }}</v-list-item-title>
              <v-list-item-subtitle>6 applications, 3 approved</v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </div>
      </v-list>
    </v-card>
  </v-menu>

</template>

<script lang="ts">
import moment from 'moment'
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import UserModule from '../../store/users'
import PersonModule from '../../store/persons'
import UpdateShiftDialog from './UpdateShiftDialog.vue'
import ShiftAssignmentDialog from './ShiftAssignmentDialog.vue'
import CreateApplicationDialog from './CreateApplicationDialog.vue'
import DeleteApplicationDialog from './DeleteApplicationDialog.vue'
import { Shift } from '../../models'

@Component({
  components: {
    UpdateShiftDialog,
    ShiftAssignmentDialog,
    CreateApplicationDialog,
    DeleteApplicationDialog,
  },
})
export default class CreateTaskDialog extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)

  @Prop(Object)
  private readonly model?: {
    model: any,
    element: any,
    event: any,
  }

  private showApplicants = true

  private get canEdit() {
    const role = this.personModule.getActiveRole
    if (role && role.calendarWrite) {
      const eligibility = role.eligibilities.find((x) => x.categoryId === this.getShift?.categoryId)
      if (eligibility) {
        return eligibility.shiftsWrite
      }
    }
    return false
  }

  private get getShift(): Shift | undefined {
    return this.model?.event.shift
  }

  private get myApplication() {
    const personId = this.personModule.getActivePersonId
    return this.getShift?.applications.find((x) => x.personId === personId)
  }

}
</script>
