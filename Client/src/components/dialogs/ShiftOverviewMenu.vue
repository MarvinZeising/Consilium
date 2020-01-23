<template>

  <v-menu
    v-model="model.model"
    :close-on-content-click="false"
    :activator="model.element"
    offset-x
  >
    <v-card min-width="350px">
      <v-toolbar
        :color="model.event.color"
        prominent
      >
        <v-toolbar-title>
          {{ userModule.getUser.formatTime(model.event.start, 'YYYY-MM-DD HH:mm') }}
        </v-toolbar-title>
        <v-spacer></v-spacer>
        <!--//TODO: check write permission -->
        <UpdateShiftDialog :shift="getShift" />
        <v-btn
          icon
          @click="model.model = false"
        >
          <v-icon>close</v-icon>
        </v-btn>
      </v-toolbar>
      <v-card-actions
        v-if="myApplication"
        class="info"
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
import CreateApplicationDialog from './CreateApplicationDialog.vue'
import DeleteApplicationDialog from './DeleteApplicationDialog.vue'
import { Shift } from '../../models'

@Component({
  components: {
    UpdateShiftDialog,
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

  private get getShift(): Shift | undefined {
    return this.model?.event.shift
  }

  private get myApplication() {
    const personId = this.personModule.getActivePersonId
    return this.getShift?.applications.find((x) => x.personId === personId)
  }

}
</script>
