<template>
  <v-container
    v-if="canView"
    fluid
    class="pa-0"
  >

    <!--//* Toolbar -->
    <!--//TODO: On mobile, replace this toolbar with a simpler one (only day view and date picker) -->
    <v-toolbar flat>
      <v-btn
        v-t="'shift.today'"
        class="mr-4"
        outlined
        @click="setToday"
      />

      <v-btn
        fab
        text
        small
        @click="$refs.calendar.prev()"
      >
        <v-icon>keyboard_arrow_left</v-icon>
      </v-btn>

      <v-btn
        fab
        text
        small
        @click="$refs.calendar.next()"
      >
        <v-icon>keyboard_arrow_right</v-icon>
      </v-btn>

      <v-toolbar-title class="ml-4">{{ getTitle }}</v-toolbar-title>

      <v-spacer />

      <CategoriesControl
        :model="categoriesModel"
        @change="(selected) => categoryModel.value = selected[0]"
      />

      <v-menu bottom right>
        <template v-slot:activator="{ on }">
          <v-btn
            v-on="on"
            outlined
          >
            <span v-t="'shift.' + type" />
            <v-icon right>arrow_drop_down</v-icon>
          </v-btn>
        </template>
        <v-list>
          <v-list-item @click="type = 'month'">
            <v-list-item-title v-t="'shift.month'" />
          </v-list-item>
          <v-list-item @click="type = 'week'">
            <v-list-item-title v-t="'shift.week'" />
          </v-list-item>
          <v-list-item @click="type = 'day'">
            <v-list-item-title v-t="'shift.day'" />
          </v-list-item>
        </v-list>
      </v-menu>

      <v-progress-linear
        :active="loading"
        :indeterminate="loading"
        absolute
        bottom
      />
    </v-toolbar>

    <!--//* Calendar -->
    <v-calendar
      id="calendar"
      ref="calendar"
      color="navbar"
      :style="`border-left:0; min-height:${getCalendarHeight}px;`"
      v-model="focus"
      :events="getEvents"
      :event-color="(event) => event.color"
      :type="type"
      :locale="userModule.getUser.language"
      :weekdays="weekdays"
      :event-more="10"
      @click:date="viewDay"
      @click:more="viewDay"
      @click:event="showEvent"
    />

    <ShiftOverviewMenu :model="shiftOverviewModel" />

    <CreateShiftDialog
      v-if="canEdit"
      :date="focus"
      :categoryModel="categoryModel"
    />

  </v-container>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import moment from 'moment'
import i18n from '../../i18n'
import UserModule from '../../store/users'
import PersonModule from '../../store/persons'
import ProjectModule from '../../store/projects'
import CategoryModule from '../../store/categories'
import TaskModule from '../../store/tasks'
import ShiftModule from '../../store/shifts'
import CreateShiftDialog from '../../components/dialogs/CreateShiftDialog.vue'
import ShiftOverviewMenu from '../../components/dialogs/ShiftOverviewMenu.vue'
import CategoriesControl from '../../components/controls/CategoriesControl.vue'
import { Shift, Category } from '../../models'

@Component({
  components: {
    CreateShiftDialog,
    ShiftOverviewMenu,
    CategoriesControl,
  }
})
export default class Calendar extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)
  private projectModule = getModule(ProjectModule, this.$store)
  private categoryModule = getModule(CategoryModule, this.$store)
  private taskModule = getModule(TaskModule, this.$store)
  private shiftModule = getModule(ShiftModule, this.$store)

  private loading: boolean = true
  private type: string = 'month'
  private focus: string = moment().format('YYYY-MM-DD')
  private events: any = []
  private weekdays: number[] = [1, 2, 3, 4, 5, 6, 0]

  private categoryModel: { value?: Category } = { value: undefined }
  private categoriesModel: { selected: Category[] } = { selected: [] }
  private shiftOverviewModel = { model: false, element: null, event: {} }

  private start: any = null
  private end: any = null

  public get canView() {
    return this.personModule.getActiveRole?.calendarRead === true
  }

  private get canEdit() {
    const role = this.personModule.getActiveRole
    if (role) {
      return role.calendarWrite === true && role.eligibilities.some((x) => x.shiftsWrite)
    }
  }

  private get getCalendarHeight() {
    return window.innerHeight - 128
  }

  private get getTitle() {
    switch (this.type) {
      case 'month':
        return moment(this.focus).format('MMMM YYYY')
      case 'week':
        const week = i18n.t('shift.week')
        return moment(this.focus).format(`[${week}] w, MMM gggg`)
      case 'day':
        return this.userModule.getUser?.formatDate(this.focus)
    }
    return ''
  }

  private get getEvents() {
    const project = this.projectModule.getActiveProject
    if (project) {
      let events: any[] = []

      // * when this gets re-evaluated, close any open shift overview menus
      this.shiftOverviewModel.model = false

      project.getCategories
        .filter((x) => this.categoriesModel.selected.find((y) => y.id === x.id))
        .forEach((x) => {
          events = events.concat(x.shifts)
        })

      events = events.map((x: Shift) => {
        const date = moment(x.date, 'YYYYMMDD').format('YYYY-MM-DD')
        const time = moment(x.time, 'Hmm').format('[T]HH:mm')
        const start = date + time
        const end = moment(start)
          .add(moment(x.duration, 'Hmm').format('H'), 'hours')
          .add(moment(x.duration, 'Hmm').format('mm'), 'minutes')
          .format('YYYY-MM-DD[T]HH:mm')

        return {
          name: x.category?.name,
          color: 'navbar',
          shift: x,
          start,
          end,
        }
      })

      return events
    }
    return []
  }

  @Watch('personModule.getActivePerson')
  private async onPersonChanged(val: string, oldVal: string) {
    if (this.canView) {
      await this.init()
    }
  }

  @Watch('$route')
  private async onRouteChanged(val: string, oldVal: string) {
    if (this.canView) {
      await this.init()
    }
  }

  private async created() {
    if (this.canView) {
      await this.init()
    }
  }

  private async init() {
    this.loading = true

    await this.categoryModule.loadCategories()
    await this.shiftModule.loadShifts()

    if (this.canEdit) {
      await this.taskModule.loadTasks()
    }

    this.categoriesModel.selected = this.projectModule.getActiveProject?.getCategories || []
    this.categoryModel.value = this.categoriesModel.selected[0]

    this.loading = false
  }

  private showEvent({ nativeEvent, event }: { nativeEvent: any, event: any }) {
    const open = () => {
      this.shiftOverviewModel.event = event
      this.shiftOverviewModel.element = nativeEvent.target
      setTimeout(() => this.shiftOverviewModel.model = true, 10)
    }

    if (this.shiftOverviewModel.model) {
      this.shiftOverviewModel.model = false
      setTimeout(open, 10)
    } else {
      open()
    }

    nativeEvent.stopPropagation()
  }

  private viewDay({ date }: { date: string }) {
    this.focus = date
    this.type = 'day'
  }

  private setToday() {
    this.focus = moment().format('YYYY-MM-DD')
  }

}
</script>
